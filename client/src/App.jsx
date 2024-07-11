import React from 'react';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom';
import SignIn from './components/SignIn';
import SignUp from './components/SignUp';
import Success from './components/Success';

function App() {
  return (
    <Router>
      <Switch>
        <Route path="/signin" component={SignIn} />
        <Route path="/signup" component={SignUp} />
        <Route path="/success" component={Success} />
        <Route path="/" exact component={SignIn} /> {/* Default route */}
      </Switch>
    </Router>
  );
}

export default App;
