class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.refs.personalNumber = React.createRef ();
        this.refs.snils = React.createRef ();
        this.handleSubmit = this.handleSubmit.bind (this);
        this.state = this.createDefaultState ();
    }

    createDefaultState () {
        return {
            isError: false,
            invalidPersonalNumber: false,
            requestWasSent: false,
            lists: []
        };
    }

    handleSubmit (e) {
        e.preventDefault ();
        const formData = new FormData (e.target);
        const data = {
            campaignTitle: formData.get ("campaignTitle"),
            snils: formData.get ("snils"),
            personalNumber: formData.get ("personalNumber")
        };

        if (!this.validateFormData (data)) {
            return;
        }

        this.props.service.getRatingLists (data,
            (results) => {
                var newState = this.createDefaultState ();
                newState.requestWasSent = true;
                newState.lists = results;
                this.setState (newState);
            },
            (xhr, status, err) => {
                console.log (xhr);
                var newState = this.createDefaultState ();
                newState.requestWasSent = true;
                newState.isError = true;
                this.setState (newState);
            }
        );
    }

    validateFormData (data) {
        if (typeof (data.personalNumber) === "undefined" || data.personalNumber === null || data.personalNumber.length === 0) {
            var newState = this.createDefaultState ();
            newState.invalidPersonalNumber = true;
            this.setState (newState);
            return false;
        }
        return true;
    }

    render () {
        return (
            <div>
                {this.renderForm ()}
                <RatingSearchResults requestWasSent={this.state.requestWasSent} lists={this.state.lists} isError={this.state.isError} />
                <hr />
                <p className="text-muted small"><a href="https://github.com/volgau/R7.Enrollment" target="_blank">R7.Enrollment v0.1</a></p>
            </div>
        );
    }

    formatCampaignTitle (campaign) {
        return campaign.CampaignTitle.replace ("21/22 ", "") + " " + campaign.CurrentDateTime;
    }

    renderForm () {
        const options = [];
        for (let campaign of this.props.campaigns) {
            options.push (<option value={campaign.CampaignTitle}>{this.formatCampaignTitle (campaign)}</option>);
        }
        if (this.props.campaigns.length === 0) {
            options.push (<option value="">-- нет данных --</option>);
        }
        return (
            <form onSubmit={this.handleSubmit} className="mb-3">
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_campaign">Приемная кампания</label>
                    <select className="form-control" name="campaignTitle" id="enrRatingSearch_campaign">
                        {options}
                    </select>
                </div>
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_snils">СНИЛС</label>
                    <input type="text" name="snils" id="enrRatingSearch_snils" ref={this.refs.snils} className="form-control" />
                </div>
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_personalNumber">Личный номер абитуриента (при отсутствии СНИЛС)</label>
                    <input type="number" min="2100000" max="2199999" name="personalNumber" id="enrRatingSearch_personalNumber"
                           ref={this.refs.personalNumber}
                           className={"form-control " + ((this.state.invalidPersonalNumber === true)? "is-invalid" : "")} />
                    {(() => {
                        if (this.state.invalidPersonalNumber === true) {
                            return (<div className="invalid-feedback">Введите личный номер абитуриента в формате 21XXXXX</div>);
                        }
                    }) ()}
                </div>
                <button type="submit" className="btn btn-primary">Найти меня в списках!</button>
            </form>
        );
    }

    componentDidMount () {
        this.refs.snils.current.value = "000-000-000-00";
        this.refs.personalNumber.current.value = "2100000";
    }
}

class RatingSearchResults extends React.Component {
    constructor (props) {
        super (props);
    }

    render () {
        if (this.props.requestWasSent === true) {
            if (this.props.lists.length > 0) {
                return this.props.lists.map (list => <div dangerouslySetInnerHTML={{__html: list.Html}} />);
            }
            if (this.props.isError === false) {
                return (
                    <p className="alert alert-warning">По вашему запросу ничего не найдено!</p>
                );
            }
            else {
                return (
                    <p className="alert alert-danger">Ой, что-то пошло не так! Перезагрузите страницу и попробуйте снова.</p>
                )
            }
        }
        return null;
    }
}

window.RatingSearch = RatingSearch;
