using OhMyDearGpnu.Api.Common.Drawing;

namespace OhMyDearGpnu;

internal class SimpleCasCaptchaResolverBestWayFindProgram
{
    internal static void Entry(string[] args)
    {
        var folder = "captcha/";
        var arrays = new List<Group>();
        for (var i = 0; i < 10; i++)
        {
            using var fs = File.OpenRead(Path.Combine(folder, $"{i}.png"));
            var image = Image.FromSimplePng(fs);
            var white = image[0, 0];
            arrays.Add(new Group($"第{i}", image.Pixels.Select(rgba => rgba == white ? 0 : 1).ToArray()));
        }

        var steps = ArraySplitter.MinStepsToSplit(arrays);
        Console.WriteLine("最少步骤数: " + steps.Count);
        Console.WriteLine("步骤: " + string.Join(", ", steps));
    }

    public record struct Group(string Name, int[] Value);

    private static class ArraySplitter
    {
        public static List<int> MinStepsToSplit(List<Group> arrays)
        {
            var steps = new List<int>();
            Split(arrays, 0, steps);
            return steps;
        }

        private static void Split(List<Group> arrays, int step, List<int> steps)
        {
            if (arrays.Count <= 1) return;

            var minSteps = int.MaxValue;
            var bestIndex = -1;
            List<Group>? bestGroup0 = null;
            List<Group>? bestGroup1 = null;

            for (var i = 0; i < arrays[0].Value.Length; i++)
            {
                var group0 = new List<Group>();
                var group1 = new List<Group>();

                foreach (var array in arrays)
                    if (array.Value[i] == 0)
                        group0.Add(array);
                    else
                        group1.Add(array);

                var steps0 = group0.Count > 0 ? group0.Count : 0;
                var steps1 = group1.Count > 0 ? group1.Count : 0;

                var maxSteps = Math.Max(steps0, steps1);
                if (maxSteps >= minSteps)
                    continue;
                minSteps = maxSteps;
                bestIndex = i;
                bestGroup0 = group0;
                bestGroup1 = group1;
            }

            steps.Add(bestIndex);
            Console.WriteLine($"经过第{step}步分割，第{bestIndex}个元素分割，分组A为：{string.Join(", ", bestGroup0?.Select(g => g.Name) ?? Array.Empty<string>())}，分组B为：{string.Join(", ", bestGroup1?.Select(g => g.Name) ?? Array.Empty<string>())}");
            Split(bestGroup0!, step + 1, steps);
            Split(bestGroup1!, step + 1, steps);
        }
    }
}