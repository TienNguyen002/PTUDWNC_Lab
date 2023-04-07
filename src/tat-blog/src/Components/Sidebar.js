import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import FeaturedPostsWidget from "./FeaturedPostsWidget";
import RandomPostsWidget from "./RandomPostsWidget";
import TagCloudWidget from "./TagCloudWidget";
import BestAuthorsWidget from "./BestAuthorsWidget";
import ArchivesWidget from "./ArchivesWidget";
import NewsletterForm from "./NewsletterForm";

const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            <SearchForm/>
            <NewsletterForm/>
            <CategoriesWidget/>
            <FeaturedPostsWidget/>
            <RandomPostsWidget/>
            <TagCloudWidget/>
            <BestAuthorsWidget/>
            <ArchivesWidget/>   
        </div>
    )
}

export default Sidebar;