using AutoFixture;
using AutoFixture.NUnit3;

namespace Tests;

public class MyFixture
{
    public static Fixture Create()
    {
        var fixture = new Fixture();
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        return fixture;
    }
    
    public class CustomAutoDataAttribute() : AutoDataAttribute(MyFixture.Create);
}