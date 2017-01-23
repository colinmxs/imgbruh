var ProgressBar = React.createClass({    
    render: function () {
        return (
        <div className='progress'>
            <div className='progress-bar progress-bar-striped active'
                    role='progressbar'
                    aria-valuenow='70'
                    aria-valuemin='0'
                    aria-valuemax='100'
                    style={{width: '100%' }}>
            <span className='sr-only'></span>
            </div>
        </div>
        );
    }
});

var ImageUpload = React.createClass({
    getInitialState: function () {
        return { showProgressBar: false, errorMessage: '', url: this.props.url };
    },
    uploadFile: function (e) {
        this.setState({ showProgressBar: true });
        var me = this;
        var fd = new FormData();
        var image = this.refs.image.getDOMNode().files[0];
        if (image.size > 5000000) {
            this.setState({ showProgressBar: false, errorMessage: 'Maximum file size is 5MB.' });
        } else {
            fd.append('image', image);
            var token = $('input[name="__RequestVerificationToken"]').val();
            fd.append('__RequestVerificationToken', token);
            $.ajax({
                url: this.state.url,
                data: fd,
                processData: false,
                contentType: false,
                type: 'POST'                
            }).done(function (data) {                
                me.setState({ errorMessage: "" });
                if (data.redirect) {
                    window.location.href = data.redirect;
                }
            }).fail(function (data, error) {
                if (data.status == 400) {
                    me.setState({ errorMessage: 'Bad Request: ' + data.responseJSON.Image.Errors[0].ErrorMessage });
                }
                else {
                    me.setState({ errorMessage: 'An error was encountered. Please try again later.' });
                }
            }).always(function (data) {
                me.setState({ showProgressBar: false });
            });
        }
        e.preventDefault()
    },    
    render: function() {
        return (
            this.state.showProgressBar ? <ProgressBar />
            :
            <div>
                <div className='text-danger'>
                    {this.state.errorMessage}
                </div>
                <br />
                <div className='btn btn-default'>
                    <input ref="image" type="file" name="image" onChange={this.uploadFile}/>
                </div>
            </div>
        );
    }
});