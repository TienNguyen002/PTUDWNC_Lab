import React, {useEffect} from "react";

const Rss = () => {
    useEffect(() => {
        document.title = 'Rss';
    }, []);

    return (
        <h1>
            Đây là Rss
        </h1>
    );
}

export default Rss;