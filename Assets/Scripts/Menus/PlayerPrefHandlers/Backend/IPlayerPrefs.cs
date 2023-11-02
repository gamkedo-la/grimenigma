public interface IPlayerPrefs
{
    string PrefName { get; }
    object PrefValue { get; set; }
}