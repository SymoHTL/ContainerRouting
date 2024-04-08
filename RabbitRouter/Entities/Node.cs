namespace RabbitRouter.Entities;

public class Node(string current, string next, string end) {
    public string Current{ get; set; } = current;
    public string Next{ get; set; } = next;
    public string End{ get; set; } = end;

    public override string ToString() {
        return $"{Current} -> {Next} -> {End}";
    }
}