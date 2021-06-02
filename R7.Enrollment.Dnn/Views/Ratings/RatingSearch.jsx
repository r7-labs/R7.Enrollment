class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.handleSubmit = this.handleSubmit.bind (this);
    }
    
    handleSubmit (e) {
        e.preventDefault ();
        var data = new FormData (e.target);
        this.props.service.getRatingLists (data,
            (result) => {
                console.log (result);
            },
            (xhr, status, err) => {
                console.log (xhr);
            }
        );
    }
    
    render () {
        return (
            <form onSubmit={this.handleSubmit}>
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_Campaign">Приемная кампания</label>
                    <select className="form-control" name="campaign" id="enrRatingSearch_Campaign">
                        <option selected="true">Бакалавриат/специалитет</option>
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_EntrantId">Идентификатор абитуриента</label>
                    <input type="text" name="entrantId" className="form-control" id="enrRatingSearch_EntrantId" />
                </div>
                <button type="submit" className="btn btn-primary">Показать списки</button>
            </form>
        );
    }
}

window.RatingSearch = RatingSearch;
