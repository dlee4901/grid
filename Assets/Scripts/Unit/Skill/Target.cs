

public class Target
{
    string selectionArea;
    enum TargetType {Select, Projectile, Beam, AOE}
    int amount;
    // Select X: Select X targets within SELECTION AREA
    // Projectile X: Select direction of X projectiles that each affect the first TARGET hit
    // AOE X: Select X areas to target (affects all units inside) in within SELECTION AREA
}