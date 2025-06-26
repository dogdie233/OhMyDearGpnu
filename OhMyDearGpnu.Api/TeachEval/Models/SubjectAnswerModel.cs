namespace OhMyDearGpnu.Api.TeachEval.Models;

public class SubjectAnswerModel
{
    public int SubjectId { get; set; }
    public List<SubjectAnswerItemModel> SubjectItems { get; set; } = [];
}

public class SubjectAnswerItemModel
{
    public int OptionId { get; set; }
    public string? ItemValue { get; set; }
}