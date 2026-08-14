namespace Application;

// Anchor type for assembly-wide scanning (e.g. MediatR registration),
// so callers don't have to depend on an arbitrary feature's handler.
public sealed class AssemblyMarker
{
    private AssemblyMarker() { }
}
