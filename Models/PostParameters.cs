namespace Sin_API.Models;

/// <summary>
/// Класс, представляющий набор свойств, необходимых для генерации синусоиды. Служит параметром для входных данных POST метода API.
/// </summary>
public class PostParameters
{
	public double A { get; set; }
	public double Fd { get; set; }
	public double Fs { get; set; }
	public int N { get; set; }
}

