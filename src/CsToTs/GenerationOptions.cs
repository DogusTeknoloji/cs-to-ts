namespace CsToTs {
    
    public class GenerationOptions: ReflectOptions {
        public static GenerationOptions Default { get; } = new GenerationOptions();

        public bool UseDateForDateTime { get; set; }

        public bool GenerateReadOnlyProps { get; set; }

        private bool GeneratePropertiesForProps { get; set; }

        public string Template { get; set; }

        private string Header { get; set; }
    }
}