class RatingSearch extends React.Component {
    constructor (props) {
        super (props);
        this.handleNoSnilsChange = this.handleNoSnilsChange.bind (this);
        this.state = {
            noSnils: false
        };
    }

    handleNoSnilsChange (noSnils) {
        this.setState ({
            noSnils: noSnils
        });
    }

    render () {
        if (this.props.campaigns.length > 0) {
            return (
                <div>
                    {this.renderDbInfo ()}
                    <RatingSearchForm
                        moduleId={this.props.moduleId}
                        service={this.props.service}
                        noSnils={this.state.noSnils}
                        onNoSnilsChange={this.handleNoSnilsChange} />
                </div>
            );
        }
        return (
            <p class="alert alert-danger">Нет данных! Перезагрузите страницу и попробуйте снова.</p>
        );
    }

    formatCampaignTitle (campaign) {
        return campaign.CampaignTitle.replace ("21/22 ", "") + " - по состоянию на " + campaign.CurrentDateTime;
    }

    renderDbInfo () {
        const items = [];
        for (let campaign of this.props.campaigns) {
            items.push (<li>{this.formatCampaignTitle (campaign)}</li>);
        }
        return (
            <div className="card card-body bg-light">
                <ul className="list-unstyled mb-0">{items}</ul>
            </div>
        );
    }
}

class RatingSearchForm extends React.Component {
    constructor (props) {
        super (props);
        this.refs.personalNumber = React.createRef ();
        this.refs.snils = React.createRef ();
        this.handleSubmit = this.handleSubmit.bind (this);
        this.handleNoSnilsChange = this.handleNoSnilsChange.bind (this);
        this.state = this.createDefaultState ();
    }

    createDefaultState () {
        return {
            isError: false,
            invalidSnils: false,
            invalidPersonalNumber: false,
            requestWasSent: false,
            lists: []
        };
    }

    handleSubmit (e) {
        e.preventDefault ();
        const formData = new FormData (e.target);
        const data = {
            snils: formData.get ("snils"),
            personalNumber: formData.get ("personalNumber")
        };

        // validate form
        const invalidSnils = !this.validateSnils (data.snils, this.props.noSnils);
        const invalidPersonalNumber = !this.validatePersonalNumber (data.personalNumber);
        if (invalidSnils || invalidPersonalNumber) {
            const newState = this.createDefaultState ();
            newState.invalidSnils = invalidSnils;
            newState.invalidPersonalNumber = invalidPersonalNumber;
            this.setState (newState);
            return;
        }

        // remove potentially invalid data before send
        if (this.props.noSnils === true) {
            data.snils = "";
        }
        else {
            data.personalNumber = "";
        }

        this.props.service.getRatingLists (data,
            (results) => {
                const newState = this.createDefaultState ();
                newState.requestWasSent = true;
                newState.lists = results;
                this.setState (newState);
            },
            (xhr, status, err) => {
                console.log (xhr);
                const newState = this.createDefaultState ();
                newState.requestWasSent = true;
                newState.isError = true;
                this.setState (newState);
            }
        );
    }

    validateSnils (snils, noSnils) {
        if (noSnils === true) {
            return true;
        }
        if (typeof (snils) === "undefined" || snils === null || snils.length === 0) {
            return false;
        }
        const snilsStripped = snils.replace (/[^\d]/g, "");
        if (snilsStripped.length !== 11) {
            return false;
        }
        return true;
    }

    validatePersonalNumber (personalNumber) {
        if (typeof (personalNumber) === "undefined" || personalNumber === null || personalNumber.length === 0) {
            return false;
        }
        return true;
    }

    handleNoSnilsChange (e) {
        console.log (e.target.checked);
        this.props.onNoSnilsChange (e.target.checked);
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

    renderForm () {
        return (
            <form onSubmit={this.handleSubmit} className="mb-3">
                <div className="form-group">
                    <label htmlFor="enrRatingSearch_snils">СНИЛС</label>
                    <input type="text" name="snils" id="enrRatingSearch_snils" ref={this.refs.snils} maxLength="64"
                           className={"form-control " + ((this.state.invalidSnils === true)? "is-invalid" : "")}
                           disabled={this.props.noSnils} />
                    {(() => {
                        if (this.state.invalidSnils === true) {
                            return (<div className="invalid-feedback">Введите СНИЛС в формате XXX-XXX-XXX-XX (11 цифр)</div>);
                        }
                    }) ()}
                </div>
                <div className="form-group">
                    <div className="form-check">
                        <input className="form-check-input" type="checkbox" id="enrRatingSearch_noSnils" onClick={this.handleNoSnilsChange} checked={this.props.noSnils} />
                        <label className="form-check-label" htmlFor="enrRatingSearch_noSnils">У меня нет СНИЛС!</label>
                    </div>
                </div>
                <div className={"form-group " + ((this.props.noSnils === false)? "d-none" : "")}>
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
