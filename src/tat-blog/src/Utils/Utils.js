import React from "react";
import { useLocation } from "react-router-dom";

export function isEmptyOrSpaces(str){
    return str === null || (typeof str === 'string' && str.match(/^ *$/) !== null);
}

export function useQuery(){
    const {search} = useLocation();

    return React.useMemo(() => 
    new URLSearchParams(search), [search]);
  }

export function isInteger(str){
    return Number.isInteger(Number(str)) && Number(str) >= 0;
}
