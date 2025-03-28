using UnityEngine;

public static class SaveMigrationManager
{
    public const int CURRENT_VERSION = 1;

    public static void Migrate(ref SaveSystem.SaveData data)
    {
        if (data.saveVersion < 1)
        {
            Debug.Log("[SaveMigration] Migrating save from unknown version.");
            data.totalCashEarned = Mathf.Max(0f, data.totalCashEarned);
            data.gems = 0;
            data.saveVersion = CURRENT_VERSION;
        }

        // Future versions:
        // if (data.saveVersion == 1) { ... migrate to 2 ... }

        Debug.Log($"[SaveMigration] Save migrated to version {CURRENT_VERSION}");
    }
}
