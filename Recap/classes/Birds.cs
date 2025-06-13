public class Birds : Animal
{
    public string? Species { get; set; }
    public override void Cry()
    {
        Console.WriteLine($"{Species}: Caw caw, bwak, bwak! Chirp..");
    }
}