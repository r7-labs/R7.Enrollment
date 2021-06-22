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
        if (this.props.campaigns.length > 0) {
            return (
                <div>
                    <RatingSearchDbInfo campaigns={this.props.campaigns} />
                    <RatingSearchForm
                        moduleId={this.props.moduleId}
                        service={this.props.service}
                        noSnils={this.state.noSnils}
                        onNoSnilsChange={this.handleNoSnilsChange} />
                </div>
            );
        }
        return (
            <RatingSearchDbInfo campaigns={this.props.campaigns} />
        );
    }
}

class RatingSearchDbInfo extends React.Component {
    constructor (props) {
        super (props);
    }

    formatCampaignTitle (campaign) {
        return campaign.CampaignTitle.replace ("21/22 ", "") + " - по состоянию на " + campaign.CurrentDateTime;
    }

    render () {
        if (this.props.campaigns.length > 0) {
            return (
                <div className="alert alert-info mb-3">
                    <h5 className="alert-heading">База данных списков абитуриентов</h5>
                    <ul className="list-unstyled mb-0">{this.getCampaignItems ()}</ul>
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

    getCampaignItems () {
        const items = [];
        for (let campaign of this.props.campaigns) {
            items.push (<li>{this.formatCampaignTitle (campaign)}</li>);
        }
        return items;
    }
}

window.RatingSearch = RatingSearch;
