class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.handleSubmit = this.handleSubmit.bind (this);
        this.state = {
            isError: false,
            requestWasSent: false,
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
                    isError: false,
                    requestWasSent: true,
                    lists: results
                });
            },
            (xhr, status, err) => {
                console.log (xhr);
                this.setState ({
                   isError: true,
                   requestWasSent: true, 
                   lists: [] 
                });
            }
        );
    }
    
    render () {
        return (
            <div>
                {this.renderForm ()}
                {this.renderLists ()}
                {this.renderError ()}
                <hr />
                <p className="text-muted small"><a href="https://github.com/volgau/R7.Enrollment" target="_blank">R7.Enrollment v0.1</a></p>
            </div>
        );
    }
    
    renderForm () {
        const options = [];
        for (let campaign of this.props.campaigns) {
            options.push (<option value={campaign.CampaignTitle}>{campaign.CampaignTitle} {campaign.CurrentDateTime}</option>);
        }
        if (this.props.campaigns.length === 0) {
            options.push (<option value="">-- нет данных --</option>);
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
                    <input type="text" name="personalNumber" className="form-control" id="enrRatingSearch_EntrantId" placeholder="2100021" />
                </div>
                <button type="submit" className="btn btn-primary">Показать списки</button>
            </form>
        );
    }
    
    renderLists () {
        if (this.state.requestWasSent === true) {
            if (this.state.lists.length > 0) {
                return this.state.lists.map(list => <div dangerouslySetInnerHTML={{__html: list.Html}}/>);
            }
            if (this.state.isError === false) {
                return (
                    <p className="alert alert-warning">По вашему запросу ничего не найдено!</p>
                );
            }
        }
    }
    
    renderError () {
        if (this.state.isError === true) {
            return (
                <p className="alert alert-danger">Ой, что-то пошло не так! Перезагрузите страницу и попробуйте снова.</p>
            )
        }
    }
}

window.RatingSearch = RatingSearch;
