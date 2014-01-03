namespace Osiris.Tfs.Monitor
{
	public class IterationWiNode : IterationNode
	{
		public override string FontWeight { get { return Selectable ? "Semibold" : "Normal"; } }

		#region Constructors

		public IterationWiNode(int? iterationId, string name, bool selectable) : this(null, iterationId, name, selectable) { }

		public IterationWiNode(IterationWiNode parent, int? iterationId, string name, bool selectable) : base(parent, iterationId, name)
		{
			this.Selectable = selectable;
		}

		#endregion // Constructors

	}
}