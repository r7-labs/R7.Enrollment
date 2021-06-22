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
        return (
            <div className="card card-body bg-light mb-3">
                <h5 className="card-title">База данных списков абитуриентов</h5>
                {this.renderDbInfo ()}
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

    renderDbInfo () {
        if (this.props.campaigns.length > 0) {
            return (
                <ul className="list-unstyled mb-0">{this.getCampaignItems ()}</ul>
            );
        }
        return (
            <p class="alert alert-danger mb-0">Не удалось получить данные! Перезагрузите страницу и попробуйте снова.</p>
        );
    }
}

window.RatingSearch = RatingSearch;
