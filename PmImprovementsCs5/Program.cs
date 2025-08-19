// CS 5 below
using System;
using System.Collections.Generic;

namespace PmImprovementsCs5 {
    public static class Program {
        static int Main() {
            List<string> names = new List<string> { "x", "y", "z" };
            var actions = new List<Action>();
            foreach (string name in names) {
                actions.Add(() => Console.WriteLine(name));
            }
            foreach (Action action in actions) {
                action();
            }

            return 0;
        }
    }
}
