package com.neurotech.brainbitneurosdkdemo.ui.resist

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.databinding.DataBindingUtil
import com.neurotech.brainbitneurosdkdemo.R
import com.neurotech.brainbitneurosdkdemo.databinding.FragmentResistBinding


class ResistFragment : Fragment() {

    companion object {
        fun newInstance() = ResistFragment()
    }

    private lateinit var binding: FragmentResistBinding
    private lateinit var viewModel: ResistViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_resist, container, false)
        viewModel = ViewModelProvider(this)[ResistViewModel::class.java]
        binding.viewModel = viewModel
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        binding.button.setOnClickListener { viewModel.onResistClicked() }
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.close()
    }

}