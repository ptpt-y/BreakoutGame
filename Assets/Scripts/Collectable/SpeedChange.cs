public class SpeedChange : Collectable
{
    public float NewSpeedMul = 2f;
    protected override void ApplyEffect(){
        BallsManager.Instance.ApplySpeedChangeEffect(NewSpeedMul);
    }
}
