import React from "react";
import SearchForm from "./SearchForm"
import NewsletterForm from "./NewsletterForm"
import CategoriesWidget from "../Widgets/CategoriesWidget"
import FeaturedPostsWidget from "../Widgets/FeaturedPostsWidget"
import RandomPostsWidget from "../Widgets/RandomPostsWidget"
import TagCloudWidget from "../Widgets/TagCloudWidget"
import BestAuthorsWidget from "../Widgets/BestAuthorsWidget"
import ArchivesWidget from "../Widgets/ArchivesWidget"

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