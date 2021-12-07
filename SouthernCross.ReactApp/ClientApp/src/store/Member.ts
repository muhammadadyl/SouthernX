import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { Member } from './shared/model/Member';
import moment from 'moment';

export interface MembersState {
    members: Member[];
    policyNumber: string;
    memberCardNumber: string;
    dateOfBirth: string;
    redirect?: string;
}

interface RequestMembersActions {
    type: 'REQUEST_MEMBERS';
    policyNumber: string;
    memberCardNumber: string;
    dateOfBirth: string;
}

interface ReceiveMembersActions {
    type: 'RECEIVE_MEMBERS';
    members: Member[];
}

interface ClearRedirectActions {
    type: 'CLEAR_REDIRECT';
}

type KnownAction = RequestMembersActions | ReceiveMembersActions | ClearRedirectActions;

export const actionCreators = {
    requestMembers: (policyNumber: string, memberCardNumber: string, dateOfBirth: string): AppThunkAction<KnownAction> => (dispatch, getState) => {
        const host = process.env.REACT_APP_SOUTHERX_URL;
        const requestOptions = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        }

        let url = `${host}/v1/members/search/${policyNumber || moment(dateOfBirth).format('DD-MM-YYYY')}`;
        if (policyNumber && memberCardNumber) {
            url += `?memberCardNumber=${encodeURIComponent(memberCardNumber)}`;
        }
        fetch(url, requestOptions)
                .then(response => response.json() as Promise<Member[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_MEMBERS', members: data });
                });

        dispatch({ type: 'REQUEST_MEMBERS', policyNumber: policyNumber, memberCardNumber: memberCardNumber, dateOfBirth: dateOfBirth  });
    },
    clearRedirect:():  AppThunkAction<KnownAction> => (dispatch, getState) => dispatch({ type: 'CLEAR_REDIRECT' })
}

const unloadedState: MembersState = {
    members: [],
    policyNumber: '',
    memberCardNumber: '',
    dateOfBirth: ''
}

export const reducer: Reducer<MembersState> = (state: MembersState | undefined, incomingAction: Action): MembersState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;

    switch (action.type) {
        case 'REQUEST_MEMBERS':
            return {
                policyNumber: action.policyNumber,
                memberCardNumber: action.memberCardNumber,
                dateOfBirth: action.dateOfBirth,
                members: state.members
            }
    
        case 'RECEIVE_MEMBERS':
            return {
                policyNumber: state.policyNumber,
                memberCardNumber: state.memberCardNumber,
                dateOfBirth: state.dateOfBirth,
                members: action.members,
                redirect: '/search-results'
            }
        
        case 'CLEAR_REDIRECT':
            return {
                policyNumber: state.policyNumber,
                memberCardNumber: state.memberCardNumber,
                dateOfBirth: state.dateOfBirth,
                members: state.members,
                redirect: undefined
            }
    }

    return state;
}