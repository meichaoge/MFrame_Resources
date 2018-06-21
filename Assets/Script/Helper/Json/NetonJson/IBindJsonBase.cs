using Newtonsoft.Json;



public interface IBindJsonBase<T>
{
    string Serialize();

    T Deserialize(string json);

    [JsonIgnore]
    string RawJson { get; set; }



}