var ButtonStyledFileInput = React.createClass({
    render: function () {
        let cssClasses = 'btn btn-default';
        return(
                <div className={cssClasses}>
                    {this.props.children}                    
                </div>
            );
    }
});

var PrimaryButton = React.createClass({
    render: function () {
        let cssClasses = 'btn btn-primary';
        return(
                <div>
                    <input className={cssClasses} type="button" ref="button" value="Upload" onClick={this.uploadFile} />                 
                </div>
            );
}
});

var ImageUpload = React.createClass({
    uploadFile: function (e) {
        var fd = new FormData();
        fd.append('image', this.refs.image.getDOMNode().files[0]);
        console.log(fd);
        var token = $('input[name="__RequestVerificationToken"]').val();
        fd.append('__RequestVerificationToken', token);
        console.log(token);

        $.ajax({
            url: 'http://localhost/imgbruh/imgs',
            data: fd,
            processData: false,
            contentType: false,
            type: 'POST',
            success: function(data){
                alert(data);
            }
        });
        e.preventDefault()
    },
    render: function() {
        return (
            <div>
                <ButtonStyledFileInput><input ref="image" type="file" name="image" /></ButtonStyledFileInput>
                <PrimaryButton />
            </div>
        );
    }
});

React.render(<ImageUpload />, document.getElementById('image-upload'))