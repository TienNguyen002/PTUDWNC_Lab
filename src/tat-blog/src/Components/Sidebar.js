import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import FeaturedPostsWidget from "./FeaturedPostsWidget";
import RandomPostsWidget from "./RandomPostsWidget";
import TagCloudWidget from "./TagCloudWidget";
import BestAuthorsWidget from "./BestAuthorsWidget";
import ArchivesWidget from "./ArchivesWidget";

const Sidebar = () => {
    return (
        <div className="pt-4 ps-2">
            <SearchForm/>
            <CategoriesWidget/>
            <FeaturedPostsWidget/>
            <RandomPostsWidget/>
            <TagCloudWidget/>
            <BestAuthorsWidget/>
            <ArchivesWidget/>
            <h1>
                Đăng ký nhận tin mới
            </h1>
            
        </div>
    )
}

export default Sidebar;