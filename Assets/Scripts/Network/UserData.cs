using Firebase.Firestore;

[FirestoreData]
public struct UserData 
{
    [FirestoreProperty]
    public float Coins { get; set; }
    [FirestoreProperty] 
    public float Diamonds { get; set; }
    [FirestoreProperty]
    public float Day { get; set; }

}
