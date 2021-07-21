<multiplechoice-component inline-template :solution="multipleChoiceJson" :highlight-empty-fields="highlightEmptyFields">
    <div class="input-container">
        <div class="overline-s no-line">Antworten</div>

        <div class="form-group" v-for="(choice, index) in choices" :key="index">
            <label class="sr-only" for="exampleInputAmount">Amount (in dollars)</label>
            <div class="input-group">
                <div @click="toggleCorrectness(index)" class="input-group-addon toggle-correctness btn is-correct grey-bg" :class="{ active: choice.IsCorrect }"><i class="fas fa-check"></i></div>
                <div @click="toggleCorrectness(index)" class="input-group-addon toggle-correctness btn is-wrong grey-bg" :class="{ active: choice.IsCorrect == false }"><i class="fas fa-times"></i></div>
                <input type="text" class="form-control" :id="'SolutionInput-'+index" placeholder="" v-model="choice.Text" v-on:change="solutionBuilder()" :class="{'is-empty' : choice.Text.length <= 0 && highlightEmptyFields}">
                <div v-if="choices.length > 1" @click="deleteChoice(index)" class="input-group-addon btn grey-bg"><i class="fas fa-trash"></i></div>
            </div>
            <div v-if="choice.Text.length <= 0 && highlightEmptyFields" class="field-error">Bitte gib eine Antwort ein.</div>
        </div>
        <div class="d-flex">
            <div @click="addChoice()" class="btn grey-bg form-control col-md-6">Antwort hinzufügen</div>
            <div class="col-sm-12 hidden-xs"></div>
        </div>
        <div class="checkbox-container">
            <div class="checkbox">
                <label> 
                    <input type="checkbox" v-model="isSolutionOrdered" :true-value="false" :false-value="true">Antworten zufällig anordnen
                </label>
            </div>
        </div>
    </div>

</multiplechoice-component>
