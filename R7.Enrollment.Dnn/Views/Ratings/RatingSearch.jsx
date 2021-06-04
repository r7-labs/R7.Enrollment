class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.handleSubmit = this.handleSubmit.bind (this);
        this.state = {
            listsLoaded: false
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
            (result) => {
                this.setState ({
                    listsLoaded: true,
                    listsMarkup: result
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
            </div>
        )
    }
    
    renderForm () {
        const options = [];
        for (let campaignTitle of this.props.campaignTitles) {
            options.push (<option>{campaignTitle}</option>);
        }
        return (
            <form onSubmit={this.handleSubmit}>
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
        if (this.state.listsLoaded === true) {
            return (
                <div dangerouslySetInnerHTML={{__html: this.state.listsMarkup}} />
            )
        }
        return null;
    }
}

window.RatingSearch = RatingSearch;
