namespace SpaceFleet.Gameplay.World;

// One world unit is one metre. Keep system coordinates precise until rendering.
public readonly record struct WorldPosition(double X, double Y, double Z)
{
    public const double MetresPerAu = 149_597_870_700;
    public static WorldPosition Zero => default;

    public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);
    public double DistanceTo(WorldPosition other) => (this - other).Length;

    public static WorldPosition operator +(WorldPosition a, WorldPosition b) =>
        new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static WorldPosition operator -(WorldPosition a, WorldPosition b) =>
        new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
}
