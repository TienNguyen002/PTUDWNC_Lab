import React, { useEffect, useState } from 'react';
import { isInteger, decode } from '../../../Utils/Utils';
import Form from    'react-bootstrap/Form'; 
import Button from 'react-bootstrap/Button'; 
import { Link , useParams , Navigate } from 'react-router-dom'; 
import { isEmptyOrSpaces }  from '../../../Utils/Utils'; 
import { addOrUpdatePost , getFilter , getPostById } from '../../../Services/BlogRepository'; 