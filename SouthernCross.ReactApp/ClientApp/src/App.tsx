import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import MemberSearch from './components/MemberSearch';
import SearchResults from './components/SearchResults';

import './custom.css'

export default () => (
    <Layout>
        <Route path='/' exact component={MemberSearch} />
        <Route path='/search-results' exact component={SearchResults} />
    </Layout>
);
