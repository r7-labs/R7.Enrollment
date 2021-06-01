class HelloWorld extends React.Component {
    render () {
        return (
            <p>Hello, world!</p>
        );
    }
}

(function ($, window, document) {
    $(() => {
        $(".enrollment-react-root").each ((i, m) => {
            ReactDOM.render (<HelloWorld />, m);
        });
    });
}) (jQuery, window, document);