namespace CsToTs {
    
    public class GenerationOptions {
        public bool UseDateForDateTime { get; set; }
        
        public static GenerationOptions Default { get; } = new GenerationOptions(); 
    }
}