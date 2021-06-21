class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.refs.personalNumber = React.createRef ();
        this.handleSubmit = this.handleSubmit.bind (this);
        this.state = {
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
            personalNumber: formData.get ("personalNumber")
        };

        if (!this.validateFormData (data)) {
            return;
        }

        this.props.service.getRatingLists (data,
            (results) => {
                this.setState ({
                    isError: false,
                    invalidPersonalNumber: false,
                    requestWasSent: true,
                    lists: results
                });
            },
            (xhr, status, err) => {
                console.log (xhr);
                this.setState ({
                   isError: true,
                   invalidPersonalNumber: false,
                   requestWasSent: true,
                   lists: []
                });
            }
        );
    }

    validateFormData (data) {
        if (typeof (data.personalNumber) === "undefined" || data.personalNumber === null || data.personalNumber.length === 0) {
            this.setState ({
                isError: false,
                invalidPersonalNumber: true,
                requestWasSent: false,
                lists: []
            });
            return false;
        }
        return true;
    }

    render () {
        return (
            <div>
                {this.renderForm ()}
                <RatingSearchResults requestWasSent={this.state.requestWasSent} lists={this.state.lists} isError={this.state.isError} />
                {this.renderError ()}
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
                    <label htmlFor="enrRatingSearch_personalNumber">Личный номер абитуриента</label>
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
        this.refs.personalNumber.current.value = "2100000";
    }

    renderError () {
        if (this.state.isError === true) {
            return (
                <p className="alert alert-danger">Ой, что-то пошло не так! Перезагрузите страницу и попробуйте снова.</p>
            )
        }
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
    }
}

window.RatingSearch = RatingSearch;
