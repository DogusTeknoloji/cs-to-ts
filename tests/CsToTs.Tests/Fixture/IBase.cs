namespace CsToTs.Tests.Fixture {
    
    public interface IBase<T> {
        T Id { get; set; }
    }

    public interface IBase: IBase<int> {
        string Name { get; set; }
    }

    public interface IBase<T, U> {
        T Id { get; set; }
        U Name { get; set; }
        IBase<T> Base1 { get; set; }
        IBase Base2 { get; set; }
    }
}