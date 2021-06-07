class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.handleSubmit = this.handleSubmit.bind (this);
        this.state = {
            lists: []
        };
    }
    
    handleSubmit (e) {
        e.preventDefault ();
        const formData = new FormData (e.target);
        const data = {
            campaignTitle: formData.get ("campaignTitle"),
            personalNumber: formData.get ("personalNumber")
        };
        this.props.service.getRatingLists (data,
            (results) => {
                this.setState ({
                    lists: results
                });
            },
            (xhr, status, err) => {
                console.log (xhr);
            }
        );
    }
    
    render () {
        return (
            <div>
                {this.renderForm ()}
                {this.renderLists ()}
                <hr />
                <p className="text-muted small"><a href="https://github.com/volgau/R7.Enrollment" target="_blank">R7.Enrollment v0.1</a></p>
            </div>
        );
    }
    
    renderForm () {
        const options = [];
        for (let campaignTitle of this.props.campaignTitles) {
            options.push (<option>{campaignTitle}</option>);
        }
        return (
            <form onSubmit={this.handleSubmit} className="mb-3">
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_Campaign">Приемная кампания</label>
                    <select className="form-control" name="campaignTitle" id="enrRatingSearch_Campaign">
                        {options}
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_EntrantId">Идентификатор абитуриента</label>
                    <input type="text" name="personalNumber" className="form-control" id="enrRatingSearch_EntrantId" value="2100021" />
                </div>
                <button type="submit" className="btn btn-primary">Показать списки</button>
            </form>
        );
    }
    
    renderLists () {
        if (this.state.lists.length > 0) {
            return this.state.lists.map (list => <div dangerouslySetInnerHTML={{__html: list.Html}} />);
        }
        return (
            <p className="alert alert-warning">По вашему запросу ничего не найдено!</p>
        );
    }
}

window.RatingSearch = RatingSearch;
