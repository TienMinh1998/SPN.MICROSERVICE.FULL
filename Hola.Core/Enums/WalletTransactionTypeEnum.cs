namespace Hola.Core.Enums
{
    public enum WalletTransactionTypeEnum
    {
        Freeze = 1,
        FreezeCommit = 2,
        FreezeRollback = 3,
        WLT2WLTTransfer = 4,
        AdminDirectTransfer = 5,
        FreezeProcess = 6,
        AdminOwnRefill = 7
    }
}