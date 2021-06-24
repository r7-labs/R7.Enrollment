import RatingSearchForm from "./RatingSearchForm.jsx";

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
        return (
            <div>
                <RatingSearchDbInfo
                    campaigns={this.props.campaigns}
                    service={this.props.service} />
                {this.renderForm ()}
                <hr className="mb-2" />
                <ul className="list-inline text-muted small">
                    <li className="list-inline-item"><a href="https://github.com/volgau/R7.Enrollment" target="_blank">R7.Enrollment v0.2</a></li>
                    <li className="list-inline-item"><a href="https://github.com/volgau/R7.Enrollment/issues/new" target="_blank">Предложить исправление</a></li>
                </ul>
            </div>
        );
    }

    renderForm () {
        if (this.props.campaigns.length > 0) {
            return (
                <RatingSearchForm
                    moduleId={this.props.moduleId}
                    service={this.props.service}
                    noSnils={this.state.noSnils}
                    onNoSnilsChange={this.handleNoSnilsChange} />
            );
        }
        return null;
    }
}

class RatingSearchDbInfo extends React.Component {
    constructor (props) {
        super (props);
    }

    render () {
        if (this.props.campaigns.length > 0) {
            return (
                <div className="alert alert-info mb-3">
                    <h5 className="alert-heading">База данных списков абитуриентов</h5>
                    <ul className="list-unstyled mb-0">
                        {this.props.campaigns.map (c => this.renderCampaign (c))}
                    </ul>
                </div>
            );
        }
        return (
            <div className="alert alert-danger mb-3">
                <h5 className="alert-heading">База данных списков абитуриентов</h5>
                <p className="mb-0">Не удалось получить данные! Перезагрузите страницу и попробуйте снова.</p>
            </div>
        );
    }

    renderCampaign (campaign) {
        return (
            <li>
                <a href={this.props.service.getUrl ("Enrollment", "GetRatingListsByCampaign", null) + "?campaignToken=" + campaign.CampaignToken}
                   className="alert-link" target="_blank">{campaign.CampaignTitle.replace ("21/22 ", "")}</a>&nbsp;&ndash;
                по состоянию на {campaign.CurrentDateTime}
            </li>
        );
    }
}

window.RatingSearch = RatingSearch;
