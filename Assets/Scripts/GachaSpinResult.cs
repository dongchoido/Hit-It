public class GachaSpinResult
{
    public bool Success { get; private set; }
    public KnifeSkinSO WonSkin { get; private set; }

    public GachaSpinResult(bool success, KnifeSkinSO wonSkin)
    {
        Success = success;
        WonSkin = wonSkin;
    }

    public static GachaSpinResult Failed()
    {
        return new GachaSpinResult(false, null);
    }
}