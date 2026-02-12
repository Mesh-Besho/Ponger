namespace MeshBesho.Ponger.Editor
	{
	internal abstract class EditorValueRequest
		{
		public String Prompt { get; set; }
		public Boolean Result { get; set; }
		public Object Value { get; set; }
		
		public EditorValueRequest(String prompt)
			{
			Prompt = prompt;
			}

		public abstract void SetResult(Object value);
		}
	
	internal class EditorValueRequest<T> : EditorValueRequest
		{
		public new T Value
			{
			get => (T)base.Value;
			}

		public Func<T, Boolean> Validator { get; set; }
		
		public EditorValueRequest(String prompt, Func<T, Boolean> validator = null)
			: base(prompt)
			{
			Prompt = prompt;
			Validator = validator;
			}

		override public void SetResult(Object value)
			{
			base.Value = Convert.ChangeType(value, typeof(T));
			}
		}
	}
