import * as React from 'react';
import { connect } from 'react-redux';
import { Redirect } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as MembersStore from '../store/Member';

type Props = MembersStore.MembersState 
  & typeof MembersStore.actionCreators;

interface States {
  policyNumber: string,
  memberCardNumber: string,
  dateOfBirth: string,
  invalid: string
}

class MemberSearch extends React.PureComponent<Props, States> {

  constructor(props: any) {
    super(props);

    this.state = { policyNumber: '', memberCardNumber: '', dateOfBirth: '', invalid: ''  };
    this.handleSubmit = this.handleSubmit.bind(this);
    this.onChange = this.onChange.bind(this);
  }

  handleSubmit(event: any) {

    if (this.state.policyNumber || this.state.dateOfBirth) {
      this.props.requestMembers(this.state.policyNumber, this.state.memberCardNumber, this.state.dateOfBirth);
    } else {
      this.setState({ invalid: 'is-invalid' });
    }

    event.preventDefault();
  }

  onChange(event: any) {
    const target = event.target;
    
    switch (target.name) {
      case 'ctrlBirthDate':
        this.setState({ dateOfBirth: target.value });
        break;
      case 'ctrlPolicyNumber':
        this.setState({ policyNumber: target.value });
        break;
      case 'ctrlMemberCardNumber':
        this.setState({ memberCardNumber: target.value });
        break;
    }

  }

  public render() {
    if (this.props.redirect) {
      return <Redirect to={this.props.redirect} />
    }
    return (
      <div>
        <h1>Member Search</h1>
        <form onSubmit={this.handleSubmit}>
          <div className="form-group">
          <label className='col-form-label-lg'>Search Using</label>
            <div className='form-group'>
              <label className='col-form-label' htmlFor="ctrlBirthDate">Date of Birth</label>
              <input className='form-control' name='ctrlBirthDate' type='date' value={this.state.dateOfBirth}  onChange={this.onChange} />
            </div>
          </div>

          <div className="form-group">
            <label className='col-form-label-lg'>Or</label>
            <div className='form-group'>
              <label className='col-form-label-md' htmlFor="ctrlPolicyNumber">Policy Number</label>
              <input className={`form-control ${this.state.invalid}`} name='ctrlPolicyNumber' type='text' pattern='\d{10}' title="Numbers only, please." value={this.state.policyNumber} onChange={this.onChange}  />
              <div className="invalid-feedback">
                Please enter policyNumber.
              </div>
            </div>
            <div className="form-group">  
              <label className='col-form-label-md' htmlFor="ctrlMemberCardNumber">Member Card Number</label>
              <input className='form-control' name='ctrlMemberCardNumber' type='text' pattern='\d{10}' title="Numbers only, please." value={this.state.memberCardNumber}  onChange={this.onChange} />
            </div>
          </div>
          <div className="form-group">
              <input type="submit" value="Submit" className="btn btn-primary btn-lg" /> 
              <input type="reset" value="Reset" className="btn btn-outline-primary btn-lg float-right" onClick={() => this.setState({ dateOfBirth: '', policyNumber: '', memberCardNumber: '' })} />
          </div>
        </form>
      </div>
    );
  }
}

export default connect(
  (state: ApplicationState) => state.members, // Selects which state properties are merged into the component's props
  MembersStore.actionCreators // Selects which action creators are merged into the component's props
)(MemberSearch as any);
