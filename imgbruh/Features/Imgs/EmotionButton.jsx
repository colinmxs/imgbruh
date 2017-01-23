var EmotionButton = React.createClass({
    render: function () {        
        return (
            <button className={this.props.btnStyle}><h3><span className={this.props.icon}></span>{this.props.buttonText}</h3></button>
        );
    }
});

var EmotionButtonSet = React.createClass({
    render: function () {
        return (
            <div className={this.props.align
            
            }>
                <EmotionButton buttonText="good" icon="glyphicon glyphicon-thumbs-up" btnStyle="btn btn-lg btn-success"></EmotionButton>
                <EmotionButton buttonText="bad" icon="glyphicon glyphicon-thumbs-down" btnStyle="btn btn-lg btn-danger"></EmotionButton>                
            </div>
            );
    }
});