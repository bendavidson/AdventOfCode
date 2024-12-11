using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day19 : DayBase
    {
        public Day19() : base("2023", "Day19") { }

        protected override void Solve()
        {
            int x = 0;

            List<Part> parts = new List<Part>();
            Dictionary<string, Tuple<List<Rule>, List<Part>>> workflows = new Dictionary<string, Tuple<List<Rule>, List<Part>>>();

            Stack<string> workflowsToProcess = new Stack<string>();

            foreach (var line in lines)
            {
                if (line.Length == 0)
                    continue;
                else if (line.StartsWith('{'))
                {
                    var part = new Part(x, line);
                    parts.Add(part);
                    x++;
                }
                else
                {
                    var lineSplit = line.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    List<Rule> rules = new List<Rule>();

                    var ruleSplit = lineSplit[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    foreach (var rule in ruleSplit)
                    {
                        var ruleComponents = rule.Split(new char[] { '<', '>', ':' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        rules.Add(new Rule
                        {
                            EqualityAttribute = ruleComponents.Length == 1 ? 'x' : ruleComponents[0][0],
                            EqualityComparer = ruleComponents.Length == 1 || rule.Contains('>') ? new GreaterThan() : new LessThan(),
                            EqualityValue = ruleComponents.Length == 1 ? 0 : Convert.ToInt32(ruleComponents[1]),
                            Result = ruleComponents.Length == 1 ? ruleComponents[0] : ruleComponents[2]
                        });
                    }

                    workflows.Add(lineSplit[0], Tuple.Create(rules, new List<Part>()));
                }
            }

            if (partNo == 1)
            {
                List<Part> partsAccepted = new List<Part>();
                List<Part> partsRejected = new List<Part>();

                // Add all parts to the list to be processed by the first workflow and add to the process list
                workflows["in"].Item2.AddRange(parts);
                workflowsToProcess.Push("in");

                while (workflowsToProcess.Count > 0)
                {
                    var workflowToProcess = workflows[workflowsToProcess.Pop()];

                    var rulesToProcess = workflowToProcess.Item1;
                    var partsToProcess = workflowToProcess.Item2;

                    foreach (var rule in rulesToProcess)
                    {
                        // Find all parts that pass the rule
                        var partsPassed = partsToProcess.Where(x => rule.EqualityComparer.Equals(x.Attributes[rule.EqualityAttribute], rule.EqualityValue)).ToList();

                        // If the rule result is Accept then add the parts to that list
                        if (rule.Result == "A")
                            partsAccepted.AddRange(partsPassed);

                        // If the rule result is Reject then add the parts to that list
                        else if (rule.Result == "R")
                            partsRejected.AddRange(partsPassed);

                        // If neither Accepted or Rejected then must be passing the parts to a new workflow
                        else
                        {
                            // Add the parts to the parts list for this new workflow
                            workflows[rule.Result].Item2.AddRange(partsPassed);

                            // Make sure the workflow is in the list of workflows to be processed
                            if (!workflowsToProcess.Contains(rule.Result))
                                workflowsToProcess.Push(rule.Result);

                        }

                        // Remove the passed parts for the list to be processed ready for the next rule
                        partsToProcess = partsToProcess.Except(partsPassed).ToList();
                    }
                }

                total = partsAccepted.Sum(p => p.Attributes.Sum(a => a.Value));
            }
            else
            {
                var acceptedWorkflows = workflows.Where(w => w.Value.Item1.Any(r => r.Result == "A")).ToList();

                List<Dictionary<char, Tuple<int, int>>> rangeCombinations = new List<Dictionary<char, Tuple<int, int>>>();

                foreach (var workflow in acceptedWorkflows)
                {
                    Dictionary<char, Tuple<int, int>> ranges = new Dictionary<char, Tuple<int, int>>();
                    ranges.Add('x', Tuple.Create(1, 4000));
                    ranges.Add('m', Tuple.Create(1, 4000));
                    ranges.Add('a', Tuple.Create(1, 4000));
                    ranges.Add('s', Tuple.Create(1, 4000));

                    var currentWorkflow = workflow;
                    string currentEntryPoint = "A";

                    var acceptedRules = currentWorkflow.Value.Item1.Where(r => r.Result == "A").ToList();

                    foreach (var acceptedRule in acceptedRules)
                    {
                        while (true)
                        {
                            var entryRule = currentWorkflow.Equals(workflow) ? acceptedRule :
                                currentWorkflow.Value.Item1.LastOrDefault(r => r.Result == currentEntryPoint);

                            for (int i = currentWorkflow.Value.Item1.IndexOf(entryRule); i >= 0; i--)
                            {
                                string prevResult = "";

                                var currentRule = currentWorkflow.Value.Item1[i];

                                if (currentRule.Result == prevResult)
                                    continue;

                                var currentRange = ranges[currentRule.EqualityAttribute];

                                if (currentRule.EqualityComparer.GetType() == typeof(GreaterThan) && currentRule == entryRule
                                    || currentRule.EqualityComparer.GetType() == typeof(LessThan) && currentRule != entryRule)
                                {
                                    ranges[currentRule.EqualityAttribute] = Tuple.Create(Math.Max(currentRange.Item1, currentRule.EqualityValue + (currentRule == entryRule ? 1 : 0)), currentRange.Item2);
                                }
                                else
                                {
                                    ranges[currentRule.EqualityAttribute] = Tuple.Create(currentRange.Item1, Math.Min(currentRange.Item2, currentRule.EqualityValue - (currentRule == entryRule ? 1 : 0)));
                                }

                                prevResult = currentRule.Result;
                            }

                            if (currentWorkflow.Key == "in")
                                break;

                            currentEntryPoint = currentWorkflow.Key;
                            currentWorkflow = workflows.FirstOrDefault(w => w.Value.Item1.Any(r => r.Result == currentEntryPoint));
                        }

                        rangeCombinations.Add(ranges);
                    }
                }

                var distanceCombinations = rangeCombinations.Distinct().ToList();

                foreach (var rangeCombination in distanceCombinations)
                {
                    long branchCombinations = (rangeCombination['x'].Item2 - (long)rangeCombination['x'].Item1 + 1)
                                                    * (rangeCombination['m'].Item2 - (long)rangeCombination['m'].Item1 + 1)
                                                    * (rangeCombination['a'].Item2 - (long)rangeCombination['a'].Item1 + 1)
                                                    * (rangeCombination['s'].Item2 - (long)rangeCombination['s'].Item1 + 1);

                    total += branchCombinations;
                }
            }
        }

        class Rule
        {
            public char EqualityAttribute { get; set; }
            public IEqualityComparer<int> EqualityComparer { get; set; }
            public int EqualityValue { get; set; }
            public string Result { get; set; }
        }

        class Part
        {
            public int Id { get; set; }
            public Dictionary<char, int> Attributes { get; set; }

            public Part(int id, string attributeString)
            {
                Id = id;
                Attributes = new Dictionary<char, int>();

                foreach (var attribute in attributeString.Split(new char[] { ',', '{', '}' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    var attributeSplit = attribute.Split('=');
                    Attributes.Add(attributeSplit[0][0], Convert.ToInt32(attributeSplit[1]));
                }
            }
        }
    }
}
