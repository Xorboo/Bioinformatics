using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BioLib.Viterbi
{
    public class ViterbiSolver
    {
        List<State> States;
        List<State> Observations;
        List<Transition> Starts;
        List<Transition> Transitions;
        List<Transition> Emissions;

        List<List<State>> Results = new List<List<State>>();

        public ViterbiSolver(List<State> states, List<State> observations, List<Transition> starts, List<Transition> transitions, List<Transition> emissions)
        {
            States = states;
            Observations = observations;
            Starts = starts;
            Transitions = transitions;
            Emissions = emissions;
        }

        public void Solve()
        {
            Results.Clear();

            ComputeFirstDay();

            for (int i = 1; i < Observations.Count; i++)
            {
                ComputeNextHighestTransitions(Observations[i]);
            }
        }

        public string ShortResults()
        {
            String s = "";
            foreach (var r in Results)
            {
                var max = r.OrderByDescending(a => a.Chance).First();
                s += max.Name + (r == Results.Last() ? "" : " -> ");
            }
            return s;
        }

        public string LongResults()
        {
            String s = "";
            foreach (var r in Results)
            {
                var max = r.OrderByDescending(a => a.Chance).First();
                foreach (var state in r)
                {
                    s += (max == state ? "♥ " : "  ") + state.Name + ": " + state.Chance + "\n";
                }
                s += "      ↓\n";
            }
            return s;
        }

        void ComputeFirstDay()
        {
            List<State> firstDayStates = new List<State>();

            var firstObservation = Observations[0];
            foreach (var state in States)
            {
                double chance = StartChance(state) * EmissionChance(state, firstObservation);
                firstDayStates.Add(new State(state.Name, chance));
            }
            Results.Add(firstDayStates);
        }

        void ComputeNextHighestTransitions(State observation)
        {
            List<State> nextStates = new List<State>();

            foreach (var nextState in States)
            {
                var newNextState = new State(nextState.Name, -1);

                foreach (var currentState in Results.Last())
                {
                    double chance = currentState.Chance * TransitionChance(currentState, nextState) * EmissionChance(nextState, observation);

                    if (chance > newNextState.Chance)
                    {
                        newNextState.Chance = chance;
                    }
                }

                nextStates.Add(newNextState);
            }
            Results.Add(nextStates);
        }

        double StartChance(State state)
        {
            return Starts.Where(a => a.To == state).First().Chance;
        }

        double EmissionChance(State state, State observation)
        {
            return Emissions.Where(a => a.From == state && a.To == observation).First().Chance;
        }

        double TransitionChance(State from, State to)
        {
            return Transitions.Where(a => a.From == from && a.To == to).First().Chance;
        }
    }
}
