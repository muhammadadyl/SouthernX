import * as React from 'react';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as MembersStore from '../store/Member';
import { Member } from '../store/shared/model/Member';

type Props = MembersStore.MembersState 
  & typeof MembersStore.actionCreators;

class SearchResults extends React.PureComponent<Props> {
    public render() {
        this.props.clearRedirect();
        if (this.props.members.length === 0)
            return (<p>no records found</p>);

        return (
            <React.Fragment>
                <h1>Search Results</h1>
                {this.props.members.map((member: Member) => 
                    <div className='card' key={member.id}>
                        <div className='card-body'>
                            <div className='row'>
                                <label className='col'>First Name</label>
                                <label className='col'>{member.firstName}</label>
                            </div>
                            <div className='row'>
                                <label className='col'>Last Name</label>
                                <label className='col'>{member.lastName}</label>
                            </div>
                            <div className='row'>
                                <label className='col'>Date of Birth</label>
                                <label className='col'>{member.dataOfBirth}</label>
                            </div>
                        </div>
                    </div>
                )}
            </React.Fragment>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.members, // Selects which state properties are merged into the component's props
    MembersStore.actionCreators // Selects which action creators are merged into the component's props
  )(SearchResults as any);