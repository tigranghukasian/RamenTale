using Firebase.Firestore;

[FirestoreData]
public struct UserData 
{
    [FirestoreProperty]
    public float Coins { get; set; }
}
