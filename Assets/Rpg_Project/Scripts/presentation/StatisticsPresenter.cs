public class StatisticsPresenter
{
    private readonly StatisticsView _view;

    public StatisticsPresenter(StatisticsView view)
    {
        _view = view;
    }

    public void Show(RunStatistics stats)
    {
        _view.Set(stats);
    }
}
