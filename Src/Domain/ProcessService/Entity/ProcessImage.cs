using ProcessService.VolueObject;

namespace ProcessService.Domain;

internal class ProcessImage
{
    public Identification ID { get; private set; }
    public Name Name { get; private set; }
    public ImagePath Path { get; private set; }
    public StatusEnum Status { get; private set; }
    public DateTime Create { get; private set; }
}